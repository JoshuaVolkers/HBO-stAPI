def pytest_addoption(parser):
    parser.addoption("--adminemail", action="store", default="default name")
    parser.addoption("--adminpass", action="store", default="default name")
    parser.addoption("--studentemail", action="store", default="default name")
    parser.addoption("--studentpass", action="store", default="default name")

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