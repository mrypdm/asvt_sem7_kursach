cmake_minimum_required(VERSION 3.12)
project(TestCppSharedLib)

set(CMAKE_CXX_STANDARD 20)

set(CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE ${CMAKE_SOURCE_DIR}/publish)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_RELEASE ${CMAKE_SOURCE_DIR}/publish)

add_library(TestCppSharedLib SHARED CoolStringBuilderExtern.cpp CoolStringBuilder.hpp)