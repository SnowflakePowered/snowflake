export default {
  'TestConfiguration': {
    'Values': {
      'BooleanOption': {
        'Value': true,
        'Guid': '800a47cc-3856-40a0-a21a-620c51e3ed41'
      },
      'StringOption': {
        'Value': 'Hello World!',
        'Guid': '42fa5b77-9604-4233-95d0-8087c5df43d7'
      },
      'EnumOption': {
        'Value': 'TestOne',
        'Guid': '0c5ae1d1-442a-40cb-a0e8-e2dba0d095c1'
      },
      'IntegerOption': {
        'Value': 0,
        'Guid': 'c82fa648-f006-4c16-8d8f-3a9f55a72974'
      },
      'DoubleOption': {
        'Value': 0.0,
        'Guid': '53e0ff95-0124-4515-9473-c8f8a80a713b'
      }
    },
    'Options': {
      'IntegerOption': {
        'Default': 0,
        'DisplayName': 'IntegerOption',
        'Description': '',
        'Simple': false,
        'CustomMetadata': {

        },
        'Type': 'integer',
        'Min': 0,
        'Max': 0,
        'Increment': 1
      },
      'DoubleOption': {
        'Default': 0.0,
        'DisplayName': 'DoubleOption',
        'Description': '',
        'Simple': false,
        'CustomMetadata': {

        },
        'Type': 'decimal',
        'Min': 0.0,
        'Max': 0.0,
        'Increment': 1.0
      },
      'BooleanOption': {
        'Default': false,
        'DisplayName': 'BooleanOption',
        'Description': '',
        'Simple': false,
        'CustomMetadata': {

        },
        'Type': 'boolean'
      },
      'StringOption': {
        'Default': 'Hello World!',
        'DisplayName': 'StringOption',
        'Description': '',
        'Simple': false,
        'CustomMetadata': {

        },
        'Type': 'string'
      },
      'EnumOption': {
        'Default': 'TestOne',
        'DisplayName': 'EnumOption',
        'Description': '',
        'Simple': false,
        'CustomMetadata': {

        },
        'Type': 'selection'
      }
    },
    'Selections': {
      'EnumOption': {
        'TestOne': {
          'DisplayName': 'TestOne',
          'Private': false
        },
        'TestTwo': {
          'DisplayName': 'TestTwo',
          'Private': false
        },
        'TestThree': {
          'DisplayName': 'TestThree',
          'Private': false
        }
      }
    }
  }
}
