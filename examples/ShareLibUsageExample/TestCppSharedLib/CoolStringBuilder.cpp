#include "CoolStringBuilder.h"

CoolStringBuilder *CoolStringBuilder::Append(const char* value) {
    _builder << value;
    return this;
}

CoolStringBuilder *CoolStringBuilder::AppendLine(const char* value) {
    _builder << value << std::endl;
    return this;
}

std::string CoolStringBuilder::ToString() const {
    return _builder.str();
}