#include "CoolStringBuilderExtern.h"

#include <iostream>
#include <cstring>

extern "C" {
void *CreateCoolStringBuilder() {
    return new CoolStringBuilder();
}

void DisposeCoolStringBuilder(void *ptr) {
    auto castedPtr = static_cast<CoolStringBuilder *>(ptr);
    delete castedPtr;
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
    free(ptr);
}
}