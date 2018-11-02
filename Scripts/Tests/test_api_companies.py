import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

base_url = "https://elbho-api-dev.azurewebsites.net/"
companies_path = "companies"

def f_test_company(company):
    v = Validator(api_schemas.company_schema)
    correct = v.validate(company)
    #print("Company schema test conducted");
    #print()
    #print(v.errors)
    assert correct == True

def f_test_base_company(base_company):
    v = Validator(api_schemas.base_company_schema)
    correct = v.validate(base_company)
    #print("Base Company schema test conducted")
    #print()
    #print(v.errors)
    assert correct == True

def f_test_max_companies(companies, max):
    correct = True
    if len(companies) > max:
        correct = False
    #print("Max companies test conducted")

    assert correct == True

def f_test_min_stars(companies, min_stars):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = True
        if base_company["averageReviewStars"] < min_stars:
            correct = False
        
        #print("Min stars test conducted")

        assert correct == True

def f_test_max_stars(companies, max_stars):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = True
        if base_company["averageReviewStars"] > max_stars:
            correct = False
        
        #print("Max stars test conducted")

        assert correct == True

def f_test_country_name(companies, country_name):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = True
        if base_company["location"]["countryName"] != country_name:
            correct = False

        #print("Country name test conducted")

        assert correct == True

def f_test_city_name(companies, city_name):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = True
        if base_company["location"]["city"] != city_name:
            correct = False

        #print("City name test conducted")

        assert correct == True

def f_test_location_range(companies, r):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = True
        if base_company["location"]["range"] > r:
            correct = False

        #print("Range test conducted")

        assert correct == True

def f_test_major(companies, major_name):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = False
        majors = base_company["majors"].split(",")

        for m in range(0, len(majors)):
            if majors[m] == major_name:
                correct = True
                break

        #print("Major test conducted")

        assert correct == True

def f_test_status_code_200(request):
    #print("Status code test conducted")
    assert request.status_code == 200

# Test multiple companies (Default)
def test_companies_default():
    companies_request = requests.get(base_url+companies_path)
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_max_companies(companies, 5)

    if len(companies) > 0:
        base_company = companies[0]
        f_test_base_company(base_company)

# Test multiple companies (detailedCompanies = true)
def test_companies_detailed():
    companies_request = requests.get(base_url+companies_path+"?detailedCompanies=true")
    f_test_status_code_200(companies_request)
    companies = companies_request.json()

    if len(companies) > 0:
        company = companies[0]
        f_test_company(company)

# Test multiple companies (minStars = 2)
def test_companies_minstars():
    companies_request = requests.get(base_url+companies_path+"?minStars=2")
    f_test_status_code_200(companies_request)
    companies = companies_request.json()

    if len(companies) > 0:
        f_test_min_stars(companies, 2)
    
# Test multiple companies (maxStars = 2)
def test_companies_maxstars():
    companies_request = requests.get(base_url+companies_path+"?maxStars=2")
    f_test_status_code_200(companies_request)
    companies = companies_request.json()

    if len(companies) > 0:
        f_test_max_stars(companies, 2)

# Test multiple companies (countryName = Nederland)
def test_companies_countryname():
    companies_request = requests.get(base_url+companies_path+"?countryName=Nederland")
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_country_name(companies, "Nederland")


# Test multiple companies (cityName = Haarlem)
def test_companies_cityname():
    companies_request = requests.get(base_url+companies_path+"?cityName=Haarlem")
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_city_name(companies, "Haarlem")

# Test multiple companies (cityName = Haarlem, locationRange = 20)
def test_companies_range():
    r = 20
    companies_request = requests.get(base_url+companies_path+"?cityName=Haarlem&locationRange="+str(r))
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    if len(companies) > 0:
        f_test_location_range(companies, r)

# Test multiple companies (major = 1)
def test_companies_major():
    major = 1
    major_name = "Informatica"
    companies_request = requests.get(base_url+companies_path+"?major="+str(major))
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_major(companies, major_name)