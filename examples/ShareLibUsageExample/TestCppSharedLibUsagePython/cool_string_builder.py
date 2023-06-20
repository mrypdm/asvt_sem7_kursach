from cool_string_builder_extern import create, dispose, append, append_line, to_string


class CoolStringBuilder:
    def __init__(self):
        self.__obj = create()

    def append(self, value: str):
        append(self.__obj, value)
        return self

    def append_line(self, value: str):
        append_line(self.__obj, value)
        return self

    def get_string(self):
        return to_string(self.__obj)

    def __del__(self):
        dispose(self.__obj)
