---
namespace: System
class: Object

---

Supports all classes in the .NET Framework class hierarchy and provides low-level services to derived classes. This is the ultimate base class of all classes in the .NET Framework; it is the root of the type hierarchy.

To browse the .NET Framework source code for this type, see the [Reference Source][1].

Remarks
-------
> **Note**
>
> To view the .NET Framework source code for this type, see the [Reference Source][1]. You can browse through the source code online, download the reference for offline viewing, and step through the sources (including patches and updates) during debugging; see [instructions][2].

Languages typically do not require a class to declare inheritance from Object because the inheritance is implicit.

Because all classes in the .NET Framework are derived from Object, every method defined in the Object class is available in all objects in the system.
Derived classes can and do override some of these methods, including:

* @Equals - Supports comparisons between objects.
* @Finalize - Performs cleanup operations before an object is automatically reclaimed.
* @GetHashCode - Generates a number corresponding to the value of the object to support the use of a hash table.
* @ToString - Manufactures a human-readable text string that describes an instance of the class.

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

Thread Safety
-------------
Public static (**Shared** in Visual Basic) members of this type are thread safe. Instance members are not guaranteed to be thread-safe.

[1]: http://referencesource.microsoft.com/#mscorlib/system/object.cs#d9262ceecc1719ab
[2]: http://referencesource.microsoft.com/

---
constructor: Object()

---

Initializes a new instance of the Object class.

This constructor is called by constructors in derived classes, but it can also be used to directly create an instance of the Object class.

---
method: Equals(Object obj)
parameters:
  obj: The object to compare with the current object.
returns: **true** if the specified object is equal to the current object; otherwise, **false**.

---

Determines whether the specified object is equal to the current object.

The type of comparison between the current instance and the *obj* parameter depends on whether the current instance is a reference type or a value type.

---
method: Equals(Object objA, Object objB)
parameters:
  objA: The first object to compare.
  objB: The second object to compare.
returns: **true** if the objects are considered equal; otherwise, **false**. If both *objA* and *objB* are **null**, the method returns **true**.

---

Determines whether the specified object instances are considered equal.

The static Equals(Object, Object) method indicates whether two objects, *objA* and *objB*, are equal. It also enables you to test objects whose value is **null** for equality. It compares *objA* and *objB* for equality as follows:

* It determines whether the two objects represent the same object reference. If they do, the method returns **true**. This test is equivalent to calling the @ReferenceEquals method. In addition, if both *objA* and *objB* are **null**, the method returns **true**.
* It determines whether either *objA* or *objB* is **null**. If so, it returns **false**.
* If the two objects do not represent the same object reference and neither is **null**, it calls *objA*.Equals(*objB*) and returns the result. This means that if *objA* overrides the @Equals(Object) method, this override is called.

---
method: Finalize()

---

Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.

The Finalize method is used to perform cleanup operations on unmanaged resources held by the current object before the object is destroyed. The method is protected and therefore is accessible only through this class or through a derived class.

---
method: GetHashCode()
returns: A hash code for the current object.

---

Serves as the default hash function.

A hash code is a numeric value that is used to insert and identify an object in a hash-based collection such as the @Dictionary<,> class, the @Hashtable class, or a type derived from the @DictionaryBase class. The GetHashCode method provides this hash code for algorithms that need quick checks of object equality.

---
method: GetType()
returns: The exact runtime type of the current instance.

---

Gets the @Type of the current instance.

For two objects *x* and *y* that have identical runtime types, `Object.ReferenceEquals(x.GetType(),y.GetType())` returns **true**. The following example uses the GetType method with the @ReferenceEquals method to determine whether one numeric value is the same type as two other numeric values.

---
method: MemberwiseClone()
returns: A shallow copy of the current @Object.

---

Creates a shallow copy of the current Object.

The MemberwiseClone method creates a shallow copy by creating a new object, and then copying the nonstatic fields of the current object to the new object. If a field is a value type, a bit-by-bit copy of the field is performed. If a field is a reference type, the reference is copied but the referred object is not; therefore, the original object and its clone refer to the same object.

For example, consider an object called X that references objects A and B. Object B, in turn, references object C. A shallow copy of X creates new object X2 that also references objects A and B. In contrast, a deep copy of X creates a new object X2 that references the new objects A2 and B2, which are copies of A and B. B2, in turn, references the new object C2, which is a copy of C. The example illustrates the difference between a shallow and a deep copy operation.

There are numerous ways to implement a deep copy operation if the shallow copy operation performed by the MemberwiseClone method does not meet your needs. These include the following:

* Call a class constructor of the object to be copied to create a second object with property values taken from the first object. This assumes that the values of an object are entirely defined by its class constructor.
* Call the MemberwiseClone method to create a shallow copy of an object, and then assign new objects whose values are the same as the original object to any properties or fields whose values are reference types. The DeepCopy method in the example illustrates this approach.
* Serialize the object to be deep copied, and then restore the serialized data to a different object variable.
* Use reflection with recursion to perform the deep copy operation.

---
method: ReferenceEquals(Object objA, Obect objB)
parameters:
  objA: The first object to compare.
  objB: The second object to compare.
returns: **true** if *objA* is the same instance as *objB* or if both are **null**; otherwise, **false**.

---

Determines whether the specified @Object instances are the same instance.

Unlike the @Equals method and the equality operator, the ReferenceEquals method cannot be overridden. Because of this, if you want to test two object references for equality and you are unsure about the implementation of the **Equals** method, you can call the ReferenceEquals method.

---
method: ToString()
returns: A string that represents the current object.

---

Returns a string that represents the current object.

ToString is the major formatting method in the .NET Framework. It converts an object to its string representation so that it is suitable for display. (For information about formatting support in the .NET Framework, see [Formatting Types in the .NET Framework](http://msdn.microsoft.com/en-us/library/26etazsy.aspx).)
