# Settings

1. Open Fastfile
   1. Change `ENV["UNITY_PRODUCT_NAME"] = "YOUR_PRODUCT_NAME_HERE"`
   2. Change `ENV["UNITY_BUNDLE_ID"] = "YOUR.BUNDLE.ID"`
   3. Change `ENV["PROV_PROFILE"] = "YOUR-HEXCODE-PROVISINGING-PROFILE-HERE"`
2. Open Matchfile
   1. Change `git_url "https://github.com/USER/YOURPROJECT.git"`
3. Open Appfile
   1. Change `app_identifier "YOUR.BUNDLE.ID"`
   2. Change `apple_id "YOUR@EMAIL.COM"`
   3. Change `team_id "DEVELOPER_PORTAL_TEAMID"`
4. Add username (`FL_UNITY_USERNAME`), password (`FL_UNITY_PASSWORD`) and serial number (`FL_UNITY_SERIAL_KEY`) to support all unity cloud features. You can do this for example through your `.bash_profile`.

