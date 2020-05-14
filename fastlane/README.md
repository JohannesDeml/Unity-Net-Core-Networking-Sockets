fastlane documentation
================
# Installation

Make sure you have the latest version of the Xcode command line tools installed:

```
xcode-select --install
```

Install _fastlane_ using
```
[sudo] gem install fastlane -NV
```
or alternatively using `brew cask install fastlane`

# Available Actions
## iOS
### ios build_unity
```
fastlane ios build_unity
```
Build Unity project for iOS
### ios build_ipa
```
fastlane ios build_ipa
```
build ipa file from Xcode project, which is ready for deployment on the AppStore
### ios upload
```
fastlane ios upload
```
Upload ipa file to AppStoreConnect and check if AppStore site is configures correctly
### ios beta
```
fastlane ios beta
```
Complete build process to get a new build for AppStoreConnect
### ios custom
```
fastlane ios custom
```
Custom pipeline for quick tests

----

This README.md is auto-generated and will be re-generated every time [fastlane](https://fastlane.tools) is run.
More information about fastlane can be found on [fastlane.tools](https://fastlane.tools).
The documentation of fastlane can be found on [docs.fastlane.tools](https://docs.fastlane.tools).
