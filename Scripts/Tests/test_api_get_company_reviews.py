import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

base_url = "https://elbho-api-dev.azurewebsites.net/"
companies_path = "companies"

def f_test_review(review):
    v = Validator(api_schemas.company_review_schema)
    correct = v.validate(review)
    print("Review schema test conducted");
    print()
    print(v.errors)
    assert correct == True

def f_test_status_code_200(request):
    print("Status code test conducted")
    assert request.status_code == 200

# Test review of company
def test_review_company():
    id=1
    company_review_request = requests.get(base_url+companies_path+"/"+str(id)+"/reviews")
    f_test_status_code_200(company_review_request)
    reviews = company_review_request.json()

    if len(reviews) > 0:
        review = reviews[0]
        f_test_review(review)