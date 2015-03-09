// Meta data used by the AngularJS docs app
angular.module('pagesData', [])
  .value('NG_PAGES', {
});
// Order matters
  angular.module('itemTypes', [])
  .value('NG_ITEMTYPES', {
  "class":[
    { "name": "Property", "description": "Property" },
    { "name": "Method" , "description": "Method"},
    { "name": "Constructor" , "description": "Constructor"},
    { "name": "Field" , "description": "Field"},
  ],
  "namespace":[
    { "name": "Class", "description": "Class" },
    { "name": "Enum" , "description": "Enum"},
    { "name": "Delegate" , "description": "Delegate"},
    { "name": "Struct" , "description": "Struct"},
    { "name": "Interface", "description": "Interface" },
  ]
});