#include <iostream>
#include <conio.h>
using namespace std;
#define A

/// Lab5_oop

#ifdef A
class Car {
protected:
	int _year;
	int _color;
	//int _mileage;
public:
	virtual void drive() { printf("Car drive\n"); }
	virtual bool isStalled() { printf("Car isStalled\n"); return false; }
	//virtual bool doFlip() = 0; //"pure" virtual
	virtual bool isA(const string& who) { return who == "Car"; }
	virtual string classname() { return "Car"; }
	Car() {
		printf("Car()\n");
	}
	Car(Car& const car) {
		_color = car._color, _year = car._year;
		printf("Car(Car& const car)\n");
	}
	Car(int year, int color) {
		_year = year, _color = color;
		printf("Car(int year, int color)\n");
	}
	virtual ~Car() {
		printf("~Car()\n");
	}
};
class Nissan : public Car {
public:
	int edition;
	int _year;
public:
	//void drive() {
	//	printf("Nissan drive\n");
	//}
	Nissan() {
		printf("Nissan()\n");
	}
	bool isA(const string& who) override {
		return who == "Car" || who == "Nissan";
	}
	bool isStalled() override {
		printf("Nissan isStalled\n");
		return false;
	}
	~Nissan() {
		printf("~Nissan()\n");
	}

};

class Peugeot : public Car {
private:
	typedef Car base;
public:
	int mileage = 0;
	string classname() override { return "Peugeot"; }
	Peugeot() {
		printf("Peugeot()\n");
	}
	void drive() {
		printf("Peugeot drive, %d\n", ++mileage);
	}
	bool isA(const string& who) override {
		printf("Peugeot isA()\n");
		return base::isA(who) || who == "Peugeot";
	}
	virtual ~Peugeot() {
		printf("~Peugeot()\n");
	}
};

class Peugeot406 : public Peugeot {
	typedef Peugeot base;
public:
	string classname() override { return "Peugeot406"; }
	//recursive checking if Peugeot406 belongs to Car
	bool isA(const string& who) override {
		return base::isA(who) || who == "Peugeot406";
	}
};
int main() {
	//Nissan n;
	//Nissan n2 = n;
	//n.isStalled();
	//n.Car::isStalled();
	Peugeot406 p4;
	printf("%d", p4.isA("Peugeot"));
	printf("\n\n");
	const int PARK_SIZE = 5;
	Car* autopark[PARK_SIZE];

	for (int i = 0; i < PARK_SIZE; i++)
	{
		if (i % 2 == 0) {
			autopark[i] = new Peugeot();
			continue;
		}
		autopark[i] = new Nissan();
	}
	for (int i = 0; i < PARK_SIZE; i++) {
		autopark[i]->drive();
		//manual safe casting (bicycle):
		//if (autopark[i]->isA("Nissan"))
		//	static_cast<Nissan*>(autopark[i])->isStalled();

		// C-style non-safe cast:
		//((Nissan*)autopark[i])->isStalled();

		//thru runtime type information (RTTI):
		Nissan* temp = dynamic_cast<Nissan*>(autopark[i]);		
		if (temp != nullptr) temp->isStalled();

		//not handy, we've to add additional checks everytime when add new child:
		//if(autopark[i]->classname() == "Peugeot"
		//	|| autopark[i]->classname() == "Peugeot406"
		//	|| autopark[i]->classname() == "Peugeot406deluxe")
		//	static_cast<Peugeot*>(autopark[i])->isStalled();

		//check if it belongs to Peugeot or not, its always Car.
		//printf("%d\n", autopark[i]->isA("Peugeot"));
		//printf("%d\n", autopark[i]->isA("Car"));
		delete autopark[i];
	}
	return 0;
}

#endif


#ifdef B

class Base {
protected:
	int x = 999;
public:
	Base() { printf("Base()\n"); }
	Base(int a) { x = a; printf("Base(int a)\n"); }
	Base(Base* b) { printf("Base(Base* b)\n"); }
	Base(const Base& b) : x(b.x) { printf("Base(const Base& b)\n"); }
	~Base() { printf("~Base()\n"); }
	virtual void Foo() { printf("Base Foo, %d\n", x); }
	virtual void setX(int _x) { x = _x; }
	virtual int getX() { return x; }
};

class Desc : public Base {
private:
	int asd = 123;
public:
	Desc(){ printf("Desc()\n"); }
	Desc(Desc* obj) { printf("Desc(Desc* obj)\n"); }
	Desc(Desc& obj) { printf("Desc(Desc& obj)\n"); }
	~Desc() { printf("~Desc()\n"); }
	void Yell() { printf("AAAAARGH, %d\n", asd); }
};


void func1(Base obj) {
	obj.Foo();
	obj.setX(111);
	printf("func1 return\n");
}
void func2(Base* obj) {
	obj->Foo();
	obj->setX(222);
	Desc* temp = dynamic_cast<Desc*>(obj);
	if (temp != nullptr) temp->Yell();
	printf("func2 return\n");
}
void func3(Base& obj) {
	obj.Foo();
	obj.setX(333);
	printf("func3 return\n");
}
//static
Base rfunc1() {
	printf("rfunc1\n");
	Base e(11);
	return e;
}

Base* rfunc2() {
	printf("rfunc2\n");
	Base e(22);
	return &e;
}

Base& rfunc3() {
	printf("rfunc3\n");
	Base e(33);
	return e;
}
//dynamic
Base rfunc4() {
	printf("rfunc4\n");
	Base* e = new Base(44);
	return *e;

}

Base* rfunc5() {
	printf("rfunc5\n");
	Base* e = new Base(55);
	return e;

}

Base& rfunc6() {
	printf("rfunc6\n");
	Base* e = new Base(66);
	return *e;

}

int main() {

	Base* e1 = new Base();
	e1->setX(666);
	printf("the x is: %d\n", e1->getX());
	//pass object copy
	func1(*e1);
	printf("func1 returned to main\n");
	printf("the x is: %d\n", e1->getX());
	
	//pass pointer
	func2(e1);		   
	printf("func2 returned to main\n");
	printf("the x is: %d\n", e1->getX());
	
	//pass dereferenced obj
	func3(*e1);			   
	printf("func3 returned to main\n");
	printf("the x is: %d\n", e1->getX());
	/////////////////////////////////////////////
	Desc* s1 = new Desc();
	s1->setX(666);
	printf("the x is: %d\n", s1->getX());
	//pass object copy
	func1(*s1);
	printf("func1 returned to main\n");
	printf("the x is: %d\n", s1->getX());
	//pass pointer
	func2(s1);
	printf("func2 returned to main\n");
	printf("the x is: %d\n", s1->getX());
	//pass dereferenced obj
	func3(*s1);
	printf("func3 returned to main\n");
	printf("the x is: %d\n", s1->getX());

	//static return
	//Base b1 = rfunc1();
	//printf("b1 got %d\n", b1.getX());
	////Base b1; b1 = rfunc1(); 
	//Base *b2 = rfunc2(); //we're pointing to empty memory area
	//printf("b2 got %d\n", b2->getX()); //garbage inside
	//Base& b3 = rfunc3(); //trying to copy when the obj is deleted
	//b3.setX(333);
	//printf("b3 got %d\n", b3.getX());
	
	////dynamic return
	//Base b4 = rfunc4(); //memory leak, we lost ptr to obj
	//b4.setX(987);
	//printf("b4 got %d\n",b4.getX());

	//Base* b5 = rfunc5(); 	//calling function is now responsible for obj's life
	//b5->Foo();
	//printf("b5 got %d\n", b5->getX());
	//delete b5;

	//Base& b6 = rfunc6(); 	//must delete after
	//b6.Foo();
	//printf("b6 got %d\n", b6.getX());
	//delete& b6;
	system("pause");
	
	return 0;
}


#endif


#ifdef C
class Base {
public:
	int a = 1337;
public:
	Base() { printf("Base()\n"); }
	Base(int a) { this->a = a; printf("Base(int a)\n"); }
	~Base() { printf("~Base()\n"); }
	void getVal() { printf("got %d\n", a); }
	void Method1() { printf("Base Method1()\n"); Method2(); }
	void Method2() { printf("Base Method2()\n"); }

	void Method3() { printf("Base Method3()\n"); Method4(); }
	virtual void Method4() { printf("Base Method4()\n"); }

	void Drift() { printf("Base Drift()\n"); }
	virtual void Run() { printf("Base Run()\n"); }

};
class Child : public Base {
	int b = 777;
public:
	Child() { printf("Child()\n"); }
	Child(int a) { this->a = a; printf("Child(int a)\n"); }
	~Child() { printf("~Base()\n"); }

	void Method2() { printf("Child Method2()\n"); }
	void Method4() override { printf("Child Method4()\n"); }

	void Drift() { printf("Child Drift()\n"); }
	void Run() override { printf("Child Run()\n"); }
};


#pragma region test_smart_methods
void smrt_un1(unique_ptr<Base> b) {
	b->Method2();
	return;
}
void smrt_sh(shared_ptr<Base> b) {
	b->Method2();
	return;
}
unique_ptr<Base> ret_unq(unique_ptr<Base> b) {
	//b->a = 345;
	return std::move(b);
}
shared_ptr<Base> ret_shd() {
	shared_ptr<Base> base = make_shared<Base>();
	return base;
}
#pragma endregion


int main() {
	Base* s2 = new Child();
	Child* s1 = new Child();
	Child s0;
	printf("\n.\n");

	s0.Method2();
	printf("\n");

	s0.Method3();
	printf("\n");

	s1->Method2();
	printf("\n");

	s2->Method2();
	printf("\n");

	s1->Method3();
	printf("\n");

	s2->Method3();
	printf("\n");
	////////////////
	s2->Drift(); //thru ptr to base
	s1->Drift(); //thru ptr to child
	printf("\n\n");
	s2->Run();
	s1->Run();

	printf("\n\n");
	//unique_ptr<Base> base1 = make_unique<Base>();
	//base1->Drift();
	//smrt_un1(std::move(base1)); 
	//shared_ptr<Base> base2 = make_shared<Base>();
	//base2->Run();
	//smrt_sh(base2);
	////unique
	//unique_ptr<Base> b1 = make_unique<Base>(81);
	//b1->getVal();
	//smrt_un1(std::move(b1));
	////b1->getVal(); //error, it's already removed by GC
	////shared
	//shared_ptr<Base> b2 = make_shared<Base>(45);
	//b2->getVal();
	//shared_ptr<Base> b3 = ret_shd();
	//b3->getVal();
	printf("\nFinished\n");
	return 0;
}

#endif


