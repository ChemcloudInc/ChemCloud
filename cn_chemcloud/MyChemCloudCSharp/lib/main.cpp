
#include <Windows.h>
#include <iostream>
#include <string>

//typedef std::string(*FNPTR)(std::string);
typedef void (*FNPTR)(char*, char**);

int main()
{
	HINSTANCE hInst = LoadLibrary(L"chemcloudDLL.dll");
	//HINSTANCE hInst = LoadLibrary(L"chemcloudDLL.dll");

	if (!hInst)
	{
		std::cout<<"\nCould not Load the library\n";
		return EXIT_FAILURE;
	}

	// Resolve function address
	FNPTR fn = (FNPTR)GetProcAddress(hInst,"Convert");
	if (!fn)
	{
		std::cout<<"\nCould not locate the function, "<<GetLastError()<<"\n";;
		return EXIT_FAILURE;
	}

	//std::string input = "\n
	char* input = "\n\
  -OEChem-04191619132D\n\
  \n\
  9  8  0     0  0  0  0  0  0999 V2000\n\
    2.5369   -0.2500    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0\n\
    3.4030    0.2500    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n\
    4.2690   -0.2500    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n\
    3.8015    0.7249    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0\n\
    3.0044    0.7249    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0\n\
    3.9590   -0.7869    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0\n\
    4.8059   -0.5600    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0\n\
    4.5790    0.2869    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0\n\
    2.0000    0.0600    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0\n\
  1  2  1  0  0  0  0\n\
  1  9  1  0  0  0  0\n\
  2  3  1  0  0  0  0\n\
  2  4  1  0  0  0  0\n\
  2  5  1  0  0  0  0\n\
  3  6  1  0  0  0  0\n\
  3  7  1  0  0  0  0\n\
  3  8  1  0  0  0  0\n\
M  END\n";

	//std::string outputStr = fn(input);
	//char output[1000];
	char* output[1000];

	fn(input, output);

	std::cout<<"\n input mol is: \n"<<std::string(input);
	std::string outputStr(*output);
	std::cout<<"\n inchi is: "<<outputStr;
	//std::cout<<"\n inchi is: "<<output;

	FreeLibrary(hInst);

	return 0;
}