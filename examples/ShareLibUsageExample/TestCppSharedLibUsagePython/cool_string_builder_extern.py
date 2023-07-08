import ctypes
import os
from typing import Callable

if os.name == "nt":
    __LIB_PATH = "../TestCppSharedLib/publish/libTestCppSharedLib.dll"
elif os.name == "posix":
    __LIB_PATH = "../TestCppSharedLib/publish/libTestCppSharedLib.so"
else:
    raise OSError("Unsupported OS")

__LIB = ctypes.CDLL(__LIB_PATH, winmode=0)

__FREE = getattr(__LIB, "FreePtr")  # type: Callable[[ctypes.c_void_p], None]
__FREE.argtypes = (ctypes.c_void_p,)

__APPEND = getattr(__LIB, "CoolStringBuilder_Append")  # type: Callable[[ctypes.c_void_p, ctypes.c_char_p], ctypes.c_void_p]
__APPEND.argtypes = (ctypes.c_void_p, ctypes.c_char_p,)
__APPEND.restype = ctypes.c_void_p

__APPEND_LINE = getattr(__LIB, "CoolStringBuilder_AppendLine")  # type: Callable[[ctypes.c_void_p, ctypes.c_char_p], ctypes.c_void_p]
__APPEND_LINE.argtypes = (ctypes.c_void_p, ctypes.c_char_p,)
__APPEND_LINE.restype = ctypes.c_void_p

__GET_STRING = getattr(__LIB, "CoolStringBuilder_ToString")  # type: Callable[[ctypes.c_void_p], ctypes.c_void_p]
__GET_STRING.argtypes = (ctypes.c_void_p,)
__GET_STRING.restype = ctypes.c_void_p

create = getattr(__LIB, "CoolStringBuilder_Create")  # type: Callable[[], ctypes.c_void_p]
create.restype = ctypes.c_void_p

dispose = getattr(__LIB, "CoolStringBuilder_Dispose")  # type: Callable[[ctypes.c_void_p], ctypes.c_uint16]
dispose.argtypes = (ctypes.c_void_p,)
dispose.restype = ctypes.c_uint16


def append(ptr: ctypes.c_void_p, value: str):
    c_str = ctypes.c_char_p(value.encode('utf-8'))
    __APPEND(ptr, c_str)


def append_line(ptr: ctypes.c_void_p, value: str):
    c_str = ctypes.c_char_p(value.encode('utf-8'))
    __APPEND_LINE(ptr, c_str)


def get_string(ptr):
    c_str = __GET_STRING(ptr)
    res = ctypes.c_char_p(c_str).value.decode('utf-8')
    __FREE(c_str)
    return res
