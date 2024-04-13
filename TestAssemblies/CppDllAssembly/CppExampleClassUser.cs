namespace CppExampleClassUserNamespace;

public class CppExampleClassUser
{
    CppExampleClass _cppExampleClass = new CppExampleClass();
}

public class CastClassA { }

/*
 * C++/CLI code contains the next .h .cpp content
CppExampleClass.h
#pragma once
public ref class CppExampleClass
{
    public:
        void DoCall();
};

CppExampleClass.cpp
#include "pch.h"
#include "CppExampleClass.h"

void CppExampleClass::DoCall()
{
}
*/
