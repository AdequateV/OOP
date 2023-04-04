#include <iostream>
#include <conio.h>

class Vector3 {
public:
	float x;
	float y;
	float z;
public:
    Vector3() {
        printf("Vector3()\n");
        x = 0, y = 0, z = 0;
    }
    Vector3(float _x, float _y, float z) {
        printf("Vector3(x,y,z)\n");
        x = _x;
        y = _y;
        this->z = z;
    }
    Vector3(const Vector3& v) {
        printf("Vector3(Vector3 v)\n");
        x = v.x,
        y = v.y,
        z = v.z;

    }
    ~Vector3() {
        printf("%.2f, %.2f, %.2f\n", x, y, z);
        printf("~Vector3()\n");
        x = 0, y = 0, z = 0;


    }
	float magnitude() {
		return (x * x + y * y + z * z);
	}
	float sqrMagnitude() {
        float number = magnitude();
        float start = 0, end = number;
        float mid, ans;

        while (start <= end) {
            mid = (start + end) / 2;
            if (mid * mid == number) {
                ans = mid;
                break;
            }
            if (mid * mid < number) {
                ans = start;
                start = mid + 1;
            }
            else {
                end = mid - 1;
            }
        }
        float increment = 0.1;
        for (int i = 0; i < 5; i++) {
            while (ans * ans <= number) {
                ans += increment;
            }
            ans =- increment;
            increment /= 10;
        }
        return ans;
        
	}
    Vector3 operator +(Vector3 const& v) {
        Vector3 res;
        res.x = v.x + x;
        res.y = v.y + y;
        res.z = v.z + z;
        return res;
    }
    Vector3 normalized() {
        float b = magnitude();
        Vector3 res(x / b, y / b, z / b);
    }
    void Normalize();
    //void Normalize() {
    //    float b = magnitude();
    //    x /= b, y /= b, z /= b;
    //}
    static Vector3 forward() {
        return Vector3(0, 0, 1);
    }
    static Vector3 left() {
        return Vector3(-1, 0, 0);
    }
    
};
void Vector3::Normalize() {
    float b = Vector3::magnitude();
    Vector3::x /= b, Vector3::y /= b, Vector3::z /= b;
}
class depressedVector3 : public Vector3 {
protected:
    float mood = 0;
public:
    depressedVector3() {
        printf("depressedVector3()\n");
        x = 0, y = 0, z = 0;
    }
    depressedVector3(float _x, float _y, float z) {
        printf("depressedVector3(x,y,z)\n");
        x = _x, y = _y;
        this->z = z;
    }
    depressedVector3(const depressedVector3& v) {
        printf("depressedVector3(Vector3 v)\n");
        x = v.x, y = v.y, z = v.z; mood = v.mood;
    }
    ~depressedVector3() {
        printf("%.2f, %.2f, %.2f, %.2f\n", x, y, z, mood);
        printf("~depressedVector3()\n");
        x = 0, y = 0, z = 0;
        mood = 0;
    }
    void changeMood(float a) {
        mood = a * 10;
    }
};

class Transform {
protected:
    Vector3* position;
    Vector3* rotation;
    Vector3* scale;
public:
    Transform() {
        printf("Transform()\n");
        position = new Vector3;
        rotation = new Vector3();
        scale = new Vector3;
    }
    Transform(Vector3& const pos, Vector3& const rot, Vector3& const sc) {
        printf("Transform(Vector3& const pos, Vector3& const rot, Vector3& const sc)\n");
        position = new Vector3(pos.x, pos.y, pos.z);
        rotation = new Vector3(rot.x, rot.y, rot.z);
        scale    = new Vector3(sc.x, sc.y, sc.z);
    }
    Transform(Transform& const t) {
        printf("Tranform(Transform &const t)\n");
        position = new Vector3(*t.position);
        rotation = new Vector3(*t.rotation);
        scale    = new Vector3(*t.scale);
    }
    ~Transform() {
        printf("~Tranform()\n");
        delete position;
        delete rotation;
        delete scale;

    }
};
class MyTransform {    
protected:
    Vector3 position;
    Vector3 rotation;
    Vector3 scale;
public:
    MyTransform() : position(), rotation(), scale() {
        printf("MyTransform()\n");
    }
    MyTransform(Vector3& const pos, Vector3& const rot, Vector3& const sc)
        : position(pos), rotation(rot), scale(sc) {
        printf("MyTransform(Vector3& const pos, Vector3& const rot, Vector3& const sc\n");
    }
    //MyTransform(MyTransform& const t)
    //    : rotation(t.rotation), position(t.position), scale(t.scale) {
    //    printf("MyTranform(Transform &const t)\n");
    //}
    MyTransform(MyTransform& const t) {
        printf("MyTranform(Transform &const t)\n");
        position = t.position;
        rotation = t.rotation;
        scale    = t.scale;
    }
    ~MyTransform() {
        printf("~MyTranform()\n");


    }

};
int main() {
    //Vector3 a(1, 2, 3);
    //Vector3* b = new Vector3(a);
    //Vector3* c = new Vector3(45, 90, 115);
    //Transform* t = new Transform(a,*b,*c);
    //delete t;
    ////b->forward;
    //depressedVector3* dv = new depressedVector3(*b);
    //dv->Normalize();
    //delete dv;
    //depressedVector3* da = new Vector3(1,99,-1337);
    Vector3* a0 = new Vector3;
    Vector3* a1 = new Vector3(1, -2, 3);
    Vector3* a2 = new Vector3(*a1);
    Vector3* a5 = new Vector3(*a1 + *a2);
    delete a0;
    delete a1;
    delete a2;
    delete a5;

    depressedVector3* d = new depressedVector3();
    Vector3* a3 = new depressedVector3(1, 2, 3);
    MyTransform* m1 = new MyTransform();
    MyTransform* m2 = new MyTransform(*m1);
    delete m1;
    delete m2;
    return 0;
}

///////////////
//
//class Point {
//public:
//    int x, y, z;
//    Point() {
//        printf("Point()\n");
//        x = 0, y = 0, z = 0;
//    }
//    Point(int x, int y, int _z) {
//        printf("Point(int x, int y, int z)\n");
//        this->x = x;
//        this->y = y;
//        z = _z;
//    }
//    Point(const Point& p) {
//        printf("Point(const Point &p)\n");
//        x = p.x, y = p.y; z = p.z;
//    }
//    ~Point() {
//        printf("%d, %d, %d\n", x, y, z);
//        printf("~Point()\n");
//    }
//    void move(int dx, int dy, int dz) {
//        x = x + dx;
//        y = y + dy;
//        z = z + dz;
//    }
//    void reset();
//};
//
//
//void Point::reset() {
//    x = 0, y = 0, z = 0;
//}
//class ColoredPoint: public Point {
//protected:
//    int R;
//    int G;
//    int B;
//public:
//    ColoredPoint() {
//        printf("Point()\n");
//        x = 0;
//        y = 0;
//        z = 0;
//
//        R = 0;
//        G = 0;
//        B = 0;
//
//    }
//    ColoredPoint(int x, int y, int _z, int R, int G, int B) {
//        printf("Point(int x, int y, int z, int RGB)\n");
//        this->x = x;
//        this->y = y;
//        z = _z;
//        this->R = R;
//        this->G = G;
//        this->B = B;
//
//    }
//    ColoredPoint(const ColoredPoint& p) {
//        printf("ColoredPoint(const ColoredPoint &p)\n");
//        x = p.x;
//        y = p.y;
//        z = p.z;
//        R = p.R;
//        G = p.G;
//        B = p.B;
//
//    }
//    ~ColoredPoint() {
//        printf("%d, %d, %d // ", x, y, z);
//        printf("R %d, G %d, B %d\n", R, G, B);
//        printf("~ColoredPoint()\n");
//    }
//    void changeColor(int _r, int _g, int _b) {
//        R = _r;
//        G = _g;
//        B = _b;
//    }
//    void reset();
//};
//class Section {
//protected:
//    Point* p1;
//    Point* p2;
//public:
//    Section() {
//        printf("Section()\n");
//        p1 = new Point;
//        p2 = new Point();
//    }
//    Section(const Section& s) {
//        printf("Point(const Section &s)\n");
//        // p1 = s.p1
//        // p2 = s.p2  -  bad 
//        p1 = new Point(*s.p1);
//        p2 = new Point(*s.p2);
//    }
//    ~Section() {
//        printf("~Section()\n");
//        delete p1;
//        delete p2;
//    }
//};
//int main()
//{
//    ColoredPoint* p = new ColoredPoint(1, 22, -1, 15, 16, 17);
//    delete p;
//}
//
