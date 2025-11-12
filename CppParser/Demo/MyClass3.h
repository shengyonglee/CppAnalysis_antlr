class MyClass :public MyBaseClass
{
public:
	MyClass();
	virtual ~MyClass();
	int getValue() const { return m_value; }
	void setValue(int val);
private:
	int m_value;

};

