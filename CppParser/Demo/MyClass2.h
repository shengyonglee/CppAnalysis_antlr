class MyBaseClass
{
public:
    void setName(const std::string& strname);
    virtual void doSomething(int param[10]);
private:
	int m_id;
	std::string m_name; // 成员变量
	int b = 1;
	vector<int> vec;
};

