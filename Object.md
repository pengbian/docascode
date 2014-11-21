apiName: System.Object

Object Class
============

Supports all classes in the .NET Framework class hierarchy and provides low-level services to derived classes. This is the ultimate base class of all classes in the .NET Framework; it is the root of the type hierarchy.

[!INCLUDE [TypeInfo](Object.TypeInfo.md)]

[!INCLUDE [Syntax](Object.Syntax.md)]

[!INCLUDE [Members](Object.Members.md)]

Remarks
-------
Languages typically do not require a class to declare inheritance from Object because the inheritance is implicit.

Because all classes in the .NET Framework are derived from Object, every method defined in the Object class is available in all objects in the system.
Derived classes can and do override some of these methods, including:

* Equals - Supports comparisons between objects.
* Finalize - Performs cleanup operations before an object is automatically reclaimed.
* GetHashCode - Generates a number corresponding to the value of the object to support the use of a hash table.
* ToString - Manufactures a human-readable text string that describes an instance of the class.

##### Performance Considerations
If you are designing a class, such as a collection, that must handle any type of object, you can create class members that accept instances of the Object class.
However, the process of boxing and unboxing a type carries a performance cost.
If you know your new class will frequently handle certain value types you can use one of two tactics to minimize the cost of boxing.

* Create a general method that accepts an Object type, and a set of type-specific method overloads that accept each value type you expect your class to frequently handle.
  If a type-specific method exists that accepts the calling parameter type, no boxing occurs and the type-specific method is invoked.
  If there is no method argument that matches the calling parameter type, the parameter is boxed and the general method is invoked.

* Design your type and its members to use generics.
  The common language runtime creates a closed generic type when you create an instance of your class and specify a generic type argument.
  The generic method is type-specific and can be invoked without boxing the calling parameter.

Although it is sometimes necessary to develop general purpose classes that accept and return Object types, you can improve performance by also providing a type-specific class to handle a frequently used type.
For example, providing a class that is specific to setting and getting Boolean values eliminates the cost of boxing and unboxing Boolean values.

Examples
--------
The following example defines a Point type derived from the Object class and overrides many of the virtual methods of the Object class.
In addition, the example shows how to call many of the static and instance methods of the Object class.

```csharp
using System;

// The Point class is derived from System.Object. 
class Point 
{
    public int x, y;

    public Point(int x, int y) 
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj) 
    {
        // If this and obj do not refer to the same type, then they are not equal. 
        if (obj.GetType() != this.GetType()) return false;

        // Return true if  x and y fields match.
        Point other = (Point) obj;
        return (this.x == other.x) && (this.y == other.y);
    }

    // Return the XOR of the x and y fields. 
    public override int GetHashCode() 
    {
        return x ^ y;
    }

    // Return the point's value as a string. 
    public override String ToString() 
    {
        return String.Format("({0}, {1})", x, y);
    }

    // Return a copy of this point object by making a simple field copy. 
    public Point Copy() 
    {
        return (Point) this.MemberwiseClone();
    }
}

public sealed class App {
    static void Main() 
    {
        // Construct a Point object.
        Point p1 = new Point(1,2);

        // Make another Point object that is a copy of the first.
        Point p2 = p1.Copy();

        // Make another variable that references the first Point object.
        Point p3 = p1;

        // The line below displays false because p1 and p2 refer to two different objects.
        Console.WriteLine(Object.ReferenceEquals(p1, p2));

        // The line below displays true because p1 and p2 refer to two different objects that have the same value.
        Console.WriteLine(Object.Equals(p1, p2));

        // The line below displays true because p1 and p3 refer to one object.
        Console.WriteLine(Object.ReferenceEquals(p1, p3));

        // The line below displays: p1's value is: (1, 2)
        Console.WriteLine("p1's value is: {0}", p1.ToString());
    }
}

// This code example produces the following output: 
// 
// False 
// True 
// True 
// p1's value is: (1, 2) 
//
```
