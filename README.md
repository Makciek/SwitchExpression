# SwitchExpression
Very simple class that helps generating expression that execute as switch
It's just kind of snippet
How to use:
```C#
  // 1st generic parameter is switch value tester, so switch(T) = int - in this case
  // 2nd value provided to lambda invoked in switch
  var ExpGen = new ExpressionGenerator<int, string>(); 
  
  // Register cases:
  ExpGen.AddCase(1, (x) => Method1(x));
  
  // generate our switch(it's autocached, since now; you can force regenerate using parameter)
  var compiled = ExpGen.Generate();
  
  // use it:
  compiled.Invoke(1, "1Test!"); // value for switch, date provided to invoked lambda
```
