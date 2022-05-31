# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## vNext

- Remove direct dependencies to JetBrains.* except SDK package (this was causing incompatibility with R# in VS 2022)

## 0.4.221

- Added missing icon to R# package

## 0.3.221

- Initial version. Includes Go to Test action (Ctrl+G+T), related test methods rename and renaming test files when
  renaming class with tests.
- Supports 2022.1.1 R#/Rider SDK
