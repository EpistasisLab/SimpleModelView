# Project Title

Simple Model View (SMV) is a Unity package meant to be a very simple implementation of a Model View-type system for simpliflying UI plumbing. 

It creates a simple paradigm for tracking and querying changes in UI elements, and getting update events when a value is changed either via UI element or code.

## Getting Started

Clone this project and import the SimpleModelView folder into your Unity project.

### Concepts

SMV uses a user-defined _mapping_ to link a value with a UI element. The user specifies one or more _mappings_ to use in a project

Each _mapping_ is represented by a single value stored in an _SMVControl_ (somewhat analagous to a controller in MVC paradigm), and by one or more _SMVView_ components that each create a link to a UI element. This way, several UI elements (via _SMVView_) can be mapped to a single value (_SMVControl) - for example if you have a 'Slider' and want a 'Text' element to show its value, or if you have both desktop and VR UI's for your program with different UI elements.

## The state of an _SMVControl_ can be changed in two ways
1) Automatically when a mapped UI element is changed.
2) Manually when your code calls 'SMV.SetValue(<mapping enum>)' with the enum corresponding to the mapping you want to change.

## Retrieving the state of an _SMVControl_

From your code, simply call 'SMV.GetValueXYZ(<mapping enum>)', where XYZ is the data type for the mapping.

## Value Change Events

Each _SMVView_ type automatically handles value-change events for the UI element it belongs to. When an event is received, it updates the _SMVControl_ that it is mapped to, which in turn updates any other _SMVView_ objects that are a part of the mapping. The _SMVControl_ then generates its own update that can be optionally handled by the user's code by supplying a method to SMV.OnUpdateEvent in the editor.

## Validation

The _SMVView_ types _SMVViewInputFieldFloat_ and _SMVViewInputFieldInt_ will only accept float and integer values, respectively, and will discard invalid input strings when input by the user, and then restore the prior valid state.

### Usage

## Add the 'SMV' singleton component to your project.
It doesn't matter where you add it. In the examples, it's in a game object called SMV.

## Define one or more mappings
Edit the members of the 'SVMmapping' enum in the file 'SMVmappings_USER_DEFINED'. Give a descriptive name to each mapping you want.

```
Give the example
```

And repeat

```
until finished
```

End with an example of getting some data out of the system or using it for a little demo

## Running the tests

Explain how to run the automated tests for this system

### Break down into end to end tests

Explain what these tests test and why

```
Give an example
```

### And coding style tests

Explain what these tests test and why

```
Give an example
```

## Deployment

Add additional notes about how to deploy this on a live system

## Built With

* [Dropwizard](http://www.dropwizard.io/1.0.2/docs/) - The web framework used
* [Maven](https://maven.apache.org/) - Dependency Management
* [ROME](https://rometools.github.io/rome/) - Used to generate RSS Feeds

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags). 

## Authors

* **Billie Thompson** - *Initial work* - [PurpleBooth](https://github.com/PurpleBooth)

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Hat tip to anyone whose code was used
* Inspiration
* etc
