.class public abstract sealed auto ansi beforefieldinit Hw2.Parser extends [System.Runtime]System.Object
{
       .method public hidebysig static void
        ParseCalcArguments(
          string[] args,
          [out] float64& val1,
          [out] valuetype Hw2.CalculatorOperation& operation,
          [out] float64& val2
        ) cil managed
      {
        .maxstack 3
        .locals init (
          [0] bool ifParsed
        )
      
        ldarg args
        call void Hw2.Parser::IsArgsLengthSupported(string[])
        
        ldarg args
        ldc.i4.0
        ldelem.ref
        ldarg val1
        call bool [System.Runtime]System.Double::TryParse(string, float64&)
        brfalse.s IL_001f
        
        ldarg args
        ldc.i4.2
        ldelem.ref
        ldarg val2
        call bool [System.Runtime]System.Double::TryParse(string, float64&)
        br.s IL_0020
        
        IL_001f: ldc.i4.0
        IL_0020: stloc.0      // ifParsed
        IL_0021: ldloc.0      // ifParsed
        IL_0022: brfalse.s    IncorrectNumber
    
        ldarg operation
        ldarg args
        ldc.i4.1
        ldelem.ref
        call valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
        stind.i4
        
        ret

        IncorrectNumber:
        ldstr "Incorrect number"
        newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
        throw
    
      }
    
    .method public static valuetype Hw2.CalculatorOperation ParseOperation(string operation)
    {
        .maxstack 4

        ldarg operation
        ldstr "+"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Plus

        ldarg operation
        ldstr "-"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Minus

        ldarg operation
        ldstr "*"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Multiply

        ldarg operation
        ldstr "/"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Divide
        
        ldc.i4.4 
        ret
        
        Plus: 
        ldc.i4.0
        ret
        
        Minus: 
        ldc.i4.1
        ret
        
        Multiply:
        ldc.i4.2
        ret
        
        Divide: 
        ldc.i4.3
        ret
    }
    
    .method private static void IsArgsLengthSupported(string[] args) cil managed
    {
        .maxstack 2
        
         ldarg args
         ldlen
         ldc.i4.3
         beq Equal
         ldstr "args"
         newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
         throw
         
         Equal: 
         ret
    }
}


