# Fork of dddnuts: https://gist.github.com/dddnuts/522302dc0b787896ebd103542372f9c1
# Fork of hadcircus: https://gist.github.com/handcircus/752eb6f51cb7a7af35aea761e74a1cf5
# Fork by JohannesDeml: https://gist.github.com/JohannesDeml/1015fd054906b70b343677609261c5e9

module Fastlane
  module Actions
    class UnityAction < Action
      def self.run(params)
        projectVersionFilePath="#{params[:project_path]}/ProjectSettings/ProjectVersion.txt"
        UI.message "Checking project version at #{projectVersionFilePath}"
        if !File.exist?(projectVersionFilePath)
             UI.error("Can't find project version file")
             return
        end

        projectVersionData = File.read(projectVersionFilePath)
        unityVersion=projectVersionData.split(": ")[1].split("\n")[0]  # Extract version from project file
        UI.message "Unity Version found in ProjectVersion: '#{unityVersion}'"
        pathToHubApplication="/Applications/Unity/Hub/Editor/#{unityVersion}/Unity.app/Contents/MacOS/Unity"

        hubApplicationFound=File.exist?(pathToHubApplication)
        UI.message "Checking for Unity Hub Application at '#{pathToHubApplication}' found #{hubApplicationFound}"
        if !hubApplicationFound
           UI.message "Can't find Unity Hub application for project version - using default path"
        end
        unityApplicationPath=hubApplicationFound ? pathToHubApplication : "/Applications/Unity/Unity.app/Contents/MacOS/Unity"

        build_cmd = "#{unityApplicationPath}"
        build_cmd << " -projectPath '#{params[:project_path]}'"
        build_cmd << " -batchmode"
        build_cmd << " -quit"

        build_cmd << " -nographics" if params[:nographics]
        build_cmd << " -logfile '#{params[:log_file]}'" if params[:log_file]

        build_cmd << " -executeMethod #{params[:execute_method]}" if params[:execute_method]

        build_cmd << " -runEditorTests" if params[:run_editor_tests]
        build_cmd << " -resultsFileDirectory=#{params[:results_file_directory]}" if params[:results_file_directory]

        build_cmd << " -username '#{params[:unity_username]}'" if params[:unity_username]
        build_cmd << " -password '#{params[:unity_password]}'" if params[:unity_password]
        build_cmd << " -serial '#{params[:unity_serial_key]}'" if params[:unity_serial_key]

        UI.message ""
        UI.message Terminal::Table.new(
          title: "Unity".green,
          headings: ["Option", "Value"],
          rows: params.values
        )
        UI.message ""

        UI.message "Start running"
        UI.message "Check out logs at \"~/Library/Logs/Unity/Editor.log\" if the build failed"
        UI.message ""

        sh build_cmd

        UI.success "Completed"
      end

      #####################################################
      # @!group Documentation
      #####################################################

      def self.description
        "Run Unity in batch mode"
      end

      def self.available_options
        [

          FastlaneCore::ConfigItem.new(key: :project_path,
                                       env_name: "FL_UNITY_PROJECT_PATH",
                                       description: "Path for Unity project",
                                       default_value: "#{Dir.pwd}"),

          FastlaneCore::ConfigItem.new(key: :log_file,
                                        env_name: "FL_UNITY_LOG_FILE",
                                        description: "Path foroutput log",
                                        default_value: "#{Dir.pwd}/Builds/log.txt"),

          FastlaneCore::ConfigItem.new(key: :execute_method,
                                       env_name: "FL_UNITY_EXECUTE_METHOD",
                                       description: "Method to execute",
                                       optional: true,
                                       default_value: nil),

          FastlaneCore::ConfigItem.new(key: :nographics,
                                       env_name: "FL_UNITY_NOGRAPHICS",
                                       description: "Initialize graphics device or not",
                                       is_string: false,
                                       default_value: true),

          FastlaneCore::ConfigItem.new(key: :run_editor_tests,
                                       env_name: "FL_UNITY_RUN_EDITOR_TESTS",
                                       description: "Option to run editor tests",
                                       is_string: false,
                                       default_value: false),

          FastlaneCore::ConfigItem.new(key: :results_file_directory,
                                       env_name: "FL_RESULTS_FILE_DIRECTORY",
                                       description: "Path for integration test results",
                                       optional: true,
                                       default_value: nil),

          FastlaneCore::ConfigItem.new(key: :unity_username,
                                        env_name: "FL_UNITY_USERNAME",
                                        description: "Username of unity account that is used to build the project",
                                        optional: true,
                                        default_value: nil),

          FastlaneCore::ConfigItem.new(key: :unity_password,
                                        env_name: "FL_UNITY_PASSWORD",
                                        description: "Password of unity account that is used to build the project",
                                        optional: true,
                                        default_value: nil),

          FastlaneCore::ConfigItem.new(key: :unity_serial_key,
                                        env_name: "FL_UNITY_SERIAL_KEY",
                                        description: "Serial key that is used to build the project",
                                        optional: true,
                                        default_value: nil)
        ]
      end

      def self.authors
        ["dddnuts"]
      end

      def self.is_supported?(platform)
        [:ios].include?(platform)
      end
    end
  end
end
