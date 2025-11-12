class MyClass :public MyBaseClass
{
public:
	int getData() const {
		return m_data;
	}
	void setData(int data) {
		m_data = data;
	}

private:
	int m_data;

};
