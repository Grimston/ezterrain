#include <iostream>
#include <string>
#include <sstream>
#include <noise/noise.h>
#include "noiseutils.h"

using namespace noise;

std::string GetName(int level, int i, int j)
{
	std::stringstream stream;
	stream <<"../../Resources/Images/l"<<level<<"c"<<i<<"_"<<j<<".bmp";

	return stream.str();
}

int main (int argc, char** argv)
{
	module::Perlin myModule;
	myModule.SetOctaveCount(module::PERLIN_MAX_OCTAVE);
	utils::NoiseMap heightMap;

	utils::NoiseMapBuilderPlane heightMapBuilder;
	heightMapBuilder.SetSourceModule (myModule);
	heightMapBuilder.SetDestNoiseMap (heightMap);
	heightMapBuilder.SetDestSize (257, 257);
	heightMapBuilder.SetBounds (2.0, 6.0, 1.0, 5.0);
	//heightMapBuilder.Build ();

	utils::RendererImage renderer;
	utils::Image image;
	renderer.SetSourceNoiseMap (heightMap);
	renderer.SetDestImage (image);
	//renderer.Render();

	utils::WriterBMP writer;
	writer.SetSourceImage (image);
	//writer.SetDestFilename ("../../tutorial.bmp");
	//writer.WriteDestFile ();

	for(int i = 0; i < 64; i++)
	{
		for(int j = 0; j < 64; j++)
		{
			std::cout << i << "-" << j << std::endl;
			std::string name = GetName(0, i, j);
			heightMapBuilder.SetBounds(i, i+1, j, j+1);
			heightMapBuilder.Build();
			renderer.Render();
			writer.SetDestFilename(name);
			writer.WriteDestFile();

			if(i%2 == 0 && j%2 == 0)
			{
				std::string name = GetName(1, i, j);
				heightMapBuilder.SetBounds(i, i+2, j, j+2);
				heightMapBuilder.Build();
				renderer.Render();
				writer.SetDestFilename(name);
				writer.WriteDestFile();
			}

			if(i%4 == 0 && j%4 == 0)
			{
				std::string name = GetName(2, i, j);
				heightMapBuilder.SetBounds(i, i+4, j, j+4);
				heightMapBuilder.Build();
				renderer.Render();
				writer.SetDestFilename(name);
				writer.WriteDestFile();
			}

			if(i%8 == 0 && j%8 == 0)
			{
				std::string name = GetName(3, i, j);
				heightMapBuilder.SetBounds(i, i+8, j, j+8);
				heightMapBuilder.Build();
				renderer.Render();
				writer.SetDestFilename(name);
				writer.WriteDestFile();
			}

			if(i%16 == 0 && j%16 == 0)
			{
				std::string name = GetName(4, i, j);
				heightMapBuilder.SetBounds(i, i+16, j, j+16);
				heightMapBuilder.Build();
				renderer.Render();
				writer.SetDestFilename(name);
				writer.WriteDestFile();
			}

			if(i%32 == 0 && j%32 == 0)
			{
				std::string name = GetName(5, i, j);
				heightMapBuilder.SetBounds(i, i+32, j, j+32);
				heightMapBuilder.Build();
				renderer.Render();
				writer.SetDestFilename(name);
				writer.WriteDestFile();
			}

			if(i%64 == 0 && j%64 == 0)
			{
				std::string name = GetName(6, i, j);
				heightMapBuilder.SetBounds(i, i+64, j, j+64);
				heightMapBuilder.Build();
				renderer.Render();
				writer.SetDestFilename(name);
				writer.WriteDestFile();
			}
		}
	}

	return 0;
}
