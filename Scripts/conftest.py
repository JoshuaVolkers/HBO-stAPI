def pytest_addoption(parser):
    parser.addoption("--adminemail", action="store", default="default name")
    parser.addoption("--adminpass", action="store", default="default name")
    parser.addoption("--studentemail", action="store", default="default name")
    parser.addoption("--studentpass", action="store", default="default name")
    parser.addoption("--apiurl", action="store", default="default name")
    parser.addoption("--jwtKey", actio="store", default="default name")
    parser.addoption("--jwtIssuer", actio="store", default="default name")
    parser.addoption("--jwtAudience", actio="store", default="default name")

def pytest_generate_tests(metafunc):
    option_value = metafunc.config.option.adminemail
    if 'adminemail' in metafunc.fixturenames and option_value is not None:
        metafunc.parametrize("adminemail", [option_value])
    
    option_value2 = metafunc.config.option.adminpass
    if 'adminpass' in metafunc.fixturenames and option_value2 is not None:
        metafunc.parametrize("adminpass", [option_value2])

    option_value3 = metafunc.config.option.studentemail
    if 'studentemail' in metafunc.fixturenames and option_value3 is not None:
        metafunc.parametrize("studentemail", [option_value3])

    option_value4 = metafunc.config.option.studentpass
    if 'studentpass' in metafunc.fixturenames and option_value4 is not None:
        metafunc.parametrize("studentpass", [option_value4])

    option_value5 = metafunc.config.option.apiurl
    if 'apiurl' in metafunc.fixturenames and option_value5 is not None:
        metafunc.parametrize("apiurl", [option_value5])

    option_value6 = metafunc.config.option.apiurl
    if 'jwtKey' in metafunc.fixturenames and option_value6 is not None:
        metafunc.parametrize("jwtKey", [option_value6])
    
    option_value7 = metafunc.config.option.apiurl
    if 'jwtIssuer' in metafunc.fixturenames and option_value7 is not None:
        metafunc.parametrize("jwtIssuer", [option_value7])

    option_value8 = metafunc.config.option.apiurl
    if 'jwtAudience' in metafunc.fixturenames and option_value8 is not None:
        metafunc.parametrize("jwtAudience", [option_value8])