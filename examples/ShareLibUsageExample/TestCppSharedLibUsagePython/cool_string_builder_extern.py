import ctypes
from typing import Callable

__LIB_PATH = "../TestCppSharedLib/publish/libTestCppSharedLib.dll"

__LIB = ctypes.CDLL(__LIB_PATH, winmode=0)

__FREE = getattr(__LIB, "FreePtr")  # type: Callable[[ctypes.c_void_p], None]
__FREE.argtypes = (ctypes.c_void_p,)

__APPEND = getattr(__LIB, "CoolStringBuilder_Append")  # type: Callable[[ctypes.c_void_p, ctypes.c_char_p], ctypes.c_void_p]
__APPEND.argtypes = (ctypes.c_void_p, ctypes.c_char_p,)
__APPEND.restype = ctypes.c_void_p

__APPEND_LINE = getattr(__LIB, "CoolStringBuilder_AppendLine")  # type: Callable[[ctypes.c_void_p, ctypes.c_char_p], ctypes.c_void_p]
__APPEND_LINE.argtypes = (ctypes.c_void_p, ctypes.c_char_p,)
__APPEND_LINE.restype = ctypes.c_void_p

__TO_STRING = getattr(__LIB, "CoolStringBuilder_ToString")  # type: Callable[[ctypes.c_void_p], ctypes.c_void_p]
__TO_STRING.argtypes = (ctypes.c_void_p,)
__TO_STRING.restype = ctypes.c_void_p

create = getattr(__LIB, "CreateCoolStringBuilder")  # type: Callable[[], ctypes.c_void_p]
create.restype = ctypes.c_void_p

dispose = getattr(__LIB, "DisposeCoolStringBuilder")  # type: Callable[[ctypes.c_void_p], None]
dispose.argtypes = (ctypes.c_void_p,)


def append(ptr: ctypes.c_void_p, value: str):
    c_str = ctypes.c_char_p(value.encode('ansi'))
    __APPEND(ptr, c_str)


def append_line(ptr: ctypes.c_void_p, value: str):
    c_str = ctypes.c_char_p(value.encode('ansi'))
    __APPEND_LINE(ptr, c_str)


def to_string(ptr):
    c_str = __TO_STRING(ptr)
    res = ctypes.c_char_p(c_str).value.decode('ansi')
    __FREE(c_str)
    return res



