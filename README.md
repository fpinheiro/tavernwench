Industry Wench
==============

Inspired on Ruby's [factory_girl](https://github.com/thoughtbot/factory_girl "A fixtures replacement library in ruby") and starting as a fork of [FactoryGirl.NET](https://github.com/JamesKovacs/FactoryGirl.NET "Minimal implementation of Ruby's factory_girl in .NET"), this project is a fixtures replacement that maps key-attributes in classes instances, ensuring that every call over the key-attribute will return the same instance. In other words, we can ask for the instance **"Johnny"** of the class **User** and the **IndustryWench** will always return the same **"Johnny"** given a context.


To configure the factory:

    IndustryWench.Configure<User>(m => {
                                       m.SetId(u => u.FirstName);
                                       m.Persist = true;
                                });


To define an actor:

    IndustryWench.Remember(() => new User {
                                  FirstName = "Johnny",
                                  LastName = "Goode"
                                });

To use a factory:

    var user = IndustryWench.Gimme<User>("Johnny");
