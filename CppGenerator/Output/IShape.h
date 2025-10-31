#ifndef _ISHAPE_H_
#define _ISHAPE_H_

#include <string>
#include <vector>


class IShape
{

public:

    IShape(); 
    
    virtual ~IShape();

    double Area() const = 0;

    double Perimeter() const = 0;

};

#endif
