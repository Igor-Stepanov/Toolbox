//open System
//
//type Operation = { apply: int -> int }
//type State = { priority: int; numberBuffern}
//
//let input = "2+3*(4+3*3)"
//
//let risePriority s = {priority = s.priority + 1}
//let lowerPriority s = {priority = s.priority - 1}
//
//let convertableToInt (ch:char) = true 
//let convertableToInt (ch:char) = 0 
//
//let toInt (x:char) = System.Convert.ToInt32(x)
//
//
//    
//    
//    
//    
//type Operator = { operation: int * int -> int }
//type Parentesis = { character: char }
//
//type Node =
//    | Operator of Operator
//    | Parentesis of Parentesis
//    | Integer of int
//
//let charToNode char =
//    match char with
//    | '+' -> { operation = (fun (x, y) -> x + y) }
//    | '-' -> { operation = (fun (x, y) -> x - y) }
//    | '/' -> { operation = (fun (x, y) -> x / y) }
//    | '*' -> { operation = (fun (x, y) -> x * y) }
//
//let calculatedExpression all =
//    all
//    |> Seq.map charToNode
//
//let calculatedExpressionFrom list =
//    match list with 
//    | [] -> None
//    | first::rest ->
//        let result = rest |> Seq.fold
//        Some result
//
//[<EntryPoint>]
//let main argv =
//    let result = calculatedExpressionFrom (Seq.toList input)    
//    printf "Result of the %s is:\n%A" input result
//    0
