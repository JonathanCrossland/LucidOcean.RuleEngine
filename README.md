# RuleEngine

Basic usage in Example console application.

## Basic Intent

The idea is simple. Provide a mechanism for "Actions" to be sequentially run, including nested actions. Actions have a name and are configured. They run in sequence, obeying the runtime. The default runtime will respond to requests, to jump to an action, to abort the sequence and so on. 

In theory, you can create a complex tree of actions, which start and then stop at a point. You can then control the sequence, jump around and build a set of rules or code that must be executed in sequence.
Thus this is called a "rule engine", since every action can be a nested set of rules.

Here is en example use case.

Validation and Rules surrounding a form post with data
- get the object or data from the form and add it to the RuleEngines Context (accessible to all actions)
- run the sequence
- first action checks for glaringly obvious large omissions, like no data was passed in, and aborts if true
- the next action runs, which looks at more specific validation points
- the next action runs, which stores the data in a json file
- the next action makes a decision based on region (a field in the data)
- the next action runs, which decides to run one of two branches
- the next action (in branch one) check if it has valid data for that region
- - If region A - triggers an email into a queue
- if region B - checks additional data
  - triggers and email into a different queue
- triggers an alert to a notifications api

Every action can contain whatever code it requires, conditional statements, switches, data read / write and then the next action is executed.

## Hints for the design

Think of a composite/command/template method pattern for a workflow pipeline.
Configurable either in app.config or by code. It would be trivial to extend this with a ServiceProvider and build actions from a database.
You can load different sequences or have a large tree, and start your execution at some point in that tree.
