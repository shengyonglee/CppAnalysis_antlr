/* @file person.h */
#ifndef _PERSON_H_
#define _PERSON_H_

#include <string>
#include <vector>

#include "Dependency1";
#include "Company1";
#include "Company2";

class Dependency1;
class Company1;
class Company2;
class Person 
{
public:
    Person();
    virtual ~Person();
    std::string getName();    void setName(const std::string& value);


protected:

private:
    std::string m_name;    int m_age;




    Company1* m_pemployer1;



    Company2* m_pemployer2;






    Address m_address;





};

#endif
