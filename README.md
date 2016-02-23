## How to Create or Add to Sections in the React Household

* [Adding data to an endpoint.](#add-data-to-endpoint)
* [Register endpoint with Flux/Redux store](#register-endpoint-with-flux/redux-store)
* [Create and register reducer](#create-and-register-reducer)
* [Add a section to the Wizard](#add-section-to-household-page)
* [Wiring redux store to the section](#wiring-redux-store-to-the-section)

### Add data to endpoint

If you are adding to an existing endpoint you can locate them here:

**_src/ExtendHealth.OneExchange/Endpoints/Non Wizard Household/Stores_**

If you are adding a new section to the household you will need to create a new endpoint in the same directory as above. It should look similar to this:

```C#
namespace ExtendHealth.OneExchange.Endpoints.Non_Wizard_Household.Stores
{
    public class NewThingStoreEndpoint
    {
        public NewThingStoreEndpoint()
        {
          //constructor. assign the endpoints dependencies.
        }

        public NewThingProfileSection get_new_thing_store(NewThingStoreInput input)
        {
          //return section model from here.
        }
    }

    public class NewThingStoreInput : ClientMessage
    {
    }

    [ClientMessage("new-thing-store-input")]
    public class NewThingProfileSection 
    {
      //define model for section
    }
}
```

Here is a simplified example of how you might build/modify the model for the new section.
```C#
public NewThingProfileSection get_new_thing_store(NewThingStoreInput input)
{
    return getNewThing();
}

public NewThingProfileSection getNewThing()
{
    var sectionData = newThingModelBuilder.BuildModel();

    return new NewThingProfileSection ()
    {
        IsNewThing= sectionData.IsNewThing,
        Name= sectionData.Name,
        IsActive = sectionData.IsActive
    };
}
```
See [CoverageOptionsStoreEndpoint](http://github.extendhealth.com/extend-health/one-exchange/blob/master/src/ExtendHealth.OneExchange/Endpoints/Non%20Wizard%20Household/Stores/CoverageOptionsProfileSection.cs) for an example implementation.

### Register endpoint with Flux/Redux store
To register an endpoint with the flux app you will want to find the root endpoint. it will have a method that looks like the following where you can register the store.
```C#
[UrlPattern("newThing")]
[AuthorizedBy(typeof(InternalUserPolicy))]
public NewThingViewModel get_newThing(NewThingInputModel inputModel)
{
    _fluxApplication.RegisterStore(typeof(NewThingProfileSection));

    return new NewThingViewModel ();
}
```

If you are adding to the household that is HouseholdEndpoint.cs which is located here: 

**_src/ExtendHealth.OneExchange/Endpoints/Non Wizard Household/HouseholdEndpoint.cs_**

See [HouseholdEndpoint.js](http://github.extendhealth.com/extend-health/one-exchange/blob/master/src/ExtendHealth.OneExchange/Endpoints/Non%20Wizard%20Household/HouseholdEndpoint.cs) for an example implementation.

### Create and register reducer

In order for the data passed from the server endpoint to the redux store to be available to a component, you will need to create a reducer. The reducer is how data is fed from the store to the components. In it's most basic form a reducer is a javascript function that takes state as a parameter and returns it. If you only need to display data then this is what you will likely need. 

```javascript
export default function name(state = {}) {
  return state;
}
```

However sometimes you will need to have a reducer that accommodates actions that signal a change to the state. In this case you will need to add a second parameter that contains the action type and the data associated with that action ```{ actionType, newData }```. 

For instance, the following example might be represented by a textbox on a page that displays the name and lets a user edit it. When the name edit is submitted you would send the appropriate action for name update ```NEWTHING_UPDATE_NAME``` and the user input ```"Jimmy"``` in an object as the second parameter to the reducer ```{NEWTHING_UPDATE_NAME, newName: "Jimmy"}```.

```javascript
import { NEWTHING_UPDATE_NAME } from '../actions/actionNames';

export default function name(state = {}, {type, newName}) {
  if (type === NEWTHING_UPDATE_NAME ) {
    return newName;
  }
  return state;
}
```

Register your reducer in the index.js file found in the directory with the reducers. If it is not in the index.js file then redux will not know to call it and your data will not show.

```javascript
export name from './name';
```
Here is the [index.js](http://redux.js.org/docs/basics/Reducers.html) file for household reducers.

If you still have questions about reducers [check out the redux docs](http://redux.js.org/docs/basics/Reducers.html).

### Wiring redux store to the section
The component that is added to the wizard needs to be wired up to use the data in the store. To wire the component up you will need to create a 'connected' version of the component that maps the data your component needs from the store to the props.

```javascript
import { connect } from 'react-redux';
import NewThing from '../new-thing/NewThing';

function mapStateToProps({ isNewThing, name, isActive}) {
  ///
}

export default connect(mapStateToProps)(NewThing);
```

The mapStateToProps function should do just what it says it does. The parameters of the function are the named properties in the root store that you need. Inside the function you map these values to a new javascript object and return it.

```javascript
function mapStateToProps({ isNewThing, name, isActive}) {
  return {
    isNewThing: isNewThing,
    name: name,
    isActive: isActive
  };
}
```
Now that it's wired up to the store you can add the section to the wizard.

### How to add a section to the Wizard
Import the component you need to add to the household. 
```javascript
import CoverageOptionsContainer from './modules/CoverageOptionsContainer';
```
Now you can add the component to the wizard.

```javascript
import CoverageOptionsContainer from './modules/CoverageOptionsContainer';

function HouseholdApp({isFundingVisible, location}) {
  const fundingContainer = isFundingVisible && <FundingContainer scrollTo="FundingOptions" title={ formatMessage('fundsAndReimbursementsTitle') } />;
  return (
    <Wizard name="Eligibility" query={ location.query } >
      <MultipleTaxHouseholdsContainer />
      <CallsToActionContainer scrollTo="Eligibility" title={ formatMessage('callsToActionTitle') } />
      <CoverageOptionsContainer scrollTo="CoverageOptions" title={ formatMessage('coverageOptionsTitle') />
      ///
    </Wizard>
  );
}
```
See [HouseholdApp.js](http://github.extendhealth.com/extend-health/one-exchange/blob/master/src/ExtendHealth.OneExchange/content/scripts/es6/household/HouseholdApp.js) for an example implementation
