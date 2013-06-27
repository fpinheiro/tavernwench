Tavern Wench
==============

Inspired on Ruby's [factory_girl](https://github.com/thoughtbot/factory_girl "A fixtures replacement library in ruby") and starting as a fork of [FactoryGirl.NET](https://github.com/JamesKovacs/FactoryGirl.NET "Minimal implementation of Ruby's factory_girl in .NET"), **Tavern Wench** implements a factory that maps key-attributes in explicitly defined object instances, assuring that every call over a value of a key-attribute will return the instance you're working with. 

Suppose you're [testing a feature](http://www.specflow.org/specflownew/ "BDD is kinda awesome") that references a *User* instance called *"Jenna"*. Every time you ask **TavernWench** for *"Jenna"* she will return the same *"Jenna"*. Here's how you do it:

First teach the wench how to deal with your class (or don't if you're fine with the [default behavior](#default_config)):

```c#
    TavernWench.Configure<User>(m => {
                                       m.SetKey(u => u.FirstName);
                                });
```

Then, explicitly declare your object:
```c#
    TavernWench.Remember(() => new User {
                                  FirstName = "Jenna",
                                  LastName = "Jameson"
                                });
```

Now ask nicely for it:
```c#
    var user = TavernWench.Gimme<User>("Jenna");
```

## Persistence Support ##



## Configuration is for phonies ##
<a id="#default_config"></a>

If you're lazy <del>like me</del> and don't want to use `TavernWench.Configure` to determine how the factory should behave, the wench will work fine as well. The default behavior is to **not persist** the objects and to use **ToString()** as the Key. So to properly get a *Fruit* with no previous configuration at all we should query the wench like this:
```c#
	public class Fruit {
        public string Name;
        public int Color { get; set; }

        public override string ToString() {
            return "A " + this.Color.ToString() + " " + this.Name;
        }
    }

    TavernWench.Remember(() => new Fruit { Name = "banana", Color = "yellow" });
    var banana = TavernWench.Gimme<Fruit>("A yellow banana");
```

## Context ##
The TavernWench will store everything you tell her to remember, but if, perchance, you want her to forget everything you said to her just invoke `TavernWench.Forget()`. That feature is specially useful if you want to prevent leaky tests.

```c#
    [SetUp]
    public void LetsTestStuff() {
        TavernWench.Remember(() => new Fruit { Name = "banana", Color = "yellow" });
    }

    [TearDown]
    public void TheTestWasAwesomeBye() {
        TavernWench.Forget();
    }
```