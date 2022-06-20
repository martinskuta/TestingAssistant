# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## 0.7.222-EAP5

- Update SDK to 2022.2.0-EAP5

## 0.7.222-EAP4

- Update SDK to 2022.2.0-EAP4
- When navigating from inner classes the Goto Test (and back) navigation action will include also containing type related types.


## 0.7.221

- When navigating from inner classes the Goto Test (and back) navigation action will include also containing type related types.

## 0.6.222-EAP4

- Updated SDK to 2022.2.0-eap04

## 0.6.222-EAP3

- Updated SDK to 2022.2.0-eap03

## 0.6.221

- Fix that Ctrl+G+T was not configured as default shortcut in R# (was working only in rider). Now R# users don't have to assign the keyboard shortcut manually.

## 0.5.221

- Remove direct dependencies to JetBrains.* except SDK package (this was causing incompatibility with R# in VS 2022)
- Now when renaming Class that has tests its related test classes will
  be renamed too and also if renaming a TestClass it will suggest to
  rename the tested class.

## 0.4.221

- Added missing icon to R# package

## 0.3.221

- Initial version. Includes Go to Test action (Ctrl+G+T), related test methods rename and renaming test files when
  renaming class with tests.
- Supports 2022.1.1 R#/Rider SDK
