from cool_string_builder_extern import create, dispose, append, append_line, get_string


class CoolStringBuilder:
    def __init__(self):
        self.__obj = create()

    @property
    def __safe_obj(self):
        if self.__obj is None:
            raise ValueError("Object is not initialized")
        return self.__obj

    def append(self, value: str):
        append(self.__safe_obj, value)
        return self

    def append_line(self, value: str):
        append_line(self.__safe_obj, value)
        return self

    def get_string(self):
        return get_string(self.__safe_obj)

    def __del__(self):
        if self.__obj is not None:
            dispose(self.__obj)
            self.__obj = None
