#ifndef _ISHAPE_H_
#define _ISHAPE_H_

#include <string>
#include <vector>


class IShape
{

public:

    IShape(); 
    
    virtual ~IShape();

    double Area() = 0;

    double Perimeter() = 0;

};

#endif
