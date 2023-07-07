#include "CoolStringBuilder.hpp"
#include "CoolStringBuilderExtern.h"

#include <iostream>
#include <cstring>

extern "C" {
void *CoolStringBuilder_Create() {
    return new CoolStringBuilder();
}

uint16_t CoolStringBuilder_Dispose(void *ptr) {
    if (!ptr) {
        return 1;
    }
    auto castedPtr = static_cast<CoolStringBuilder *>(ptr);
    delete castedPtr;
    return 0;
}

void *CoolStringBuilder_Append(void *ptr, const char *value) {
    auto castedPtr = static_cast<CoolStringBuilder *>(ptr);
    return castedPtr->Append(value);
}

void *CoolStringBuilder_AppendLine(void *ptr, const char *value) {
    auto castedPtr = static_cast<CoolStringBuilder *>(ptr);
    return castedPtr->AppendLine(value);
}

char *CoolStringBuilder_ToString(void *ptr) {
    auto castedPtr = static_cast<CoolStringBuilder *>(ptr);
    auto res = strdup(castedPtr->ToString().c_str());
    return res;
}

void FreePtr(void* ptr) {
    if (ptr) {
        free(ptr);
    }
}
}