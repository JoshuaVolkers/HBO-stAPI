import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator
import json

users_path = "users"

def f_test_login(token):
    correct = isinstance(token, str)
    assert correct == True