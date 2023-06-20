#pragma once

#include <sstream>

class CoolStringBuilder {
private:
    std::stringstream _builder;

public:
	// In `Extern "C"` we cannot use templates
    CoolStringBuilder *Append(const char* value);

    CoolStringBuilder *AppendLine(const char* value);

    std::string ToString() const;
};
