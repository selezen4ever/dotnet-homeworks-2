.class public abstract sealed auto ansi beforefieldinit Hw2.Calculator extends System.Object
{ 
    .method static public hidebysig float64 Calculate(float64 value1,
     valuetype Hw2.CalculatorOperation operation, float64 value2)
     cil managed       
     {
        ldarg operation
        switch (Plus, Minus, Multiply, Divide, Undefined)
        
        Plus: 
        ldarg value1
        ldarg value2 
        add 
        ret
        
        Minus: 
        ldarg value1 
        ldarg value2
        sub 
        ret
       
        Multiply:
        ldarg value1 
        ldarg value2
        mul 
        ret
        
        Divide: 
        ldarg value1 
        ldarg value2
        brzero ZeroDivision
        ldarg value2
        div 
        ret
        
        Undefined:
        ldstr "operation" 
        newobj instance void [System.Runtime]System.InvalidOperationException::.ctor(string)
        throw
        
        ZeroDivision:
        newobj instance void [System.Runtime]System.DivideByZeroException::.ctor()
        throw
             
     }
}



