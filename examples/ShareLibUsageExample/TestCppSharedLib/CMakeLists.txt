cmake_minimum_required(VERSION 3.24)
project(TestCppSharedLib)

set(CMAKE_CXX_STANDARD 20)

set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_SOURCE_DIR}/publish)

add_library(TestCppSharedLib SHARED CoolStringBuilderExtern.cpp CoolStringBuilder.cpp)
