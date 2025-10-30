#ifndef _PERSON_H_
#define _PERSON_H_

#include <string>
#include <vector>

#include "Dependency1"
#include "Company1"
#include "Company2"
#include "Company3"
#include "Company4"

class Dependency1;
class Company1;
class Company2;
class Company3;
class Company4;
class Person
{

public:

    Person(); 
    
    virtual ~Person();

    std::string getName();

    std::string namea = "aaa";

    static int a;

    Company1* pemployer1;

protected:

    void setName(const std::string& value);

    int b = 10;

    std::vector<Company2*> employer2;

    Company3* pemployer3;

private:

    string setName1(const std::string& value);

    Company4* pemployer4;

    Address address;

};

#endif
