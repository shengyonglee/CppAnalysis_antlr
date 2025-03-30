#ifndef _PERSON_H_
#define _PERSON_H_

#include <string>
#include <vector>


class Person
{

public:

    Person(); 
    
    virtual ~Person();

    std::string getName();

    void setName(const std::string& v);

    std::string name = "Tom";

private:

    int age;

};

#endif
