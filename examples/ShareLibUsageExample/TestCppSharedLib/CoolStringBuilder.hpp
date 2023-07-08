#pragma once

#include <sstream>

class CoolStringBuilder {
private:
    std::stringstream _builder;

public:
    template <class T>
    CoolStringBuilder *Append(T&& value) {
        _builder << std::forward<T>(value);
        return this;
    }

    template <class T>
    CoolStringBuilder *AppendLine(T&& value) {
        _builder << std::forward<T>(value) << std::endl;
        return this;
    }

    std::string ToString() const {
        return _builder.str();
    }
};