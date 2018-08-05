
using System;
using Todo.Models;

public class UserGrainState
{
    public Guid guid {get;set;}
    public TodoItem[] items {get;set;}
}