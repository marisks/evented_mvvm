namespace ScoreModel

[<AutoOpen>]
module ScoreTypes =
    type Score = { ScoreA: int ; ScoreB: int }    
    type ScoringEvent = IncA | DecA | IncB | DecB | New

module Score =
    let zero = {ScoreA = 0; ScoreB = 0}
    let adjustScore score event =
       match event with
        | IncA -> {score with ScoreA = score.ScoreA + 1}
        | DecA -> {score with ScoreA = max (score.ScoreA - 1) 0}
        | IncB -> {score with ScoreB = score.ScoreB + 1}
        | DecB -> {score with ScoreB = max (score.ScoreB - 1) 0}
        | New -> zero 
