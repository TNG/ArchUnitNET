![ArchUnit][logo]
[logo]: ArchUnit-Logo.png

# API

To understand the ArchUnitNET Framework we first recommend reading about how we
import C# files and save the corresponing information.

### ArchUnitNET Taxonomy

The analyzed C# bytecode is imported into the ARchUnitNET C# code structure.
Our taxonomies are described [here](guide/taxonomies.md).


### Architecture

To load your Project into your Testingframework you first have to create an ArchLoader

### Predicates

* [Object Predicates](guide/predicate/object.md)

	* [Type Predicates](guide/predicate/type.md)
		* [Attribute Predicates](guide/predicate/attribute.md)
		* [Class Predicates](guide/predicate/class.md)
		* [Interface Predicates](guide/predicate/interface.md)

	* [Member Predicates](guide/predicate/member.md)
		* [FieldMember Predicates](guide/predicate/fieldmember.md)
		* [MethodMember Predicates](guide/predicate/methodmember.md)
		* [PropertyMember Predicates](guide/predicate/propertymember.md) 


### Conditions

* [Object Conditions](guide/condition/object.md)

	* [Type Conditions](guide/condition/type.md)
		* [Attribute Conditions](guide/condition/attribute.md)
		* [Class Conditions](guide/condition/class.md)
		* [Interface Conditions](guide/condition/interface.md)

	* [Member Conditions](guide/condition/member.md)
		* [FieldMember Conditions](guide/condition/fieldmember.md)
		* [MethodMember Conditions](guide/condition/methodmember.md)
		* [PropertyMember Conditions](guide/condition/propertymember.md) 


## Further Examples
Check out example code here
[ArchUnitNET Examples](https://github.com/TNG/ArchUnitNET/tree/master/ExampleTest "ExampleTests").