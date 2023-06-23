#pragma once

#include "CoolStringBuilder.h"

extern "C" {
void *CoolStringBuilder_Create();

uint16_t CoolStringBuilder_Dispose(void *ptr);

void *CoolStringBuilder_Append(void *ptr, const char *value);

void *CoolStringBuilder_AppendLine(void *ptr, const char *value);

char *CoolStringBuilder_ToString(void *ptr);

void FreePtr(void* ptr);
}