#include <iostream>
#include <string>
#include <sstream>
#include <noise/noise.h>
#include "noiseutils.h"

using namespace noise;

#define MAX_LEVEL (6)

std::string GetName(int level, int i, int j)
{
	std::stringstream stream;
	stream <<"../../Resources/Images/l"<<level<<"c"<<i<<"_"<<j<<".bmp";

	return stream.str();
}

void Build(utils::NoiseMapBuilderPlane& builder, 
		   utils::RendererImage& renderer,
		   utils::WriterBMP& writer,
		   int level,
		   int i, int j)
{
	int step = 1 << level;

	if(i % step == 0 && j % step == 0)
	{
		std::string name = GetName(level, i, j);
		std::cout << name << std::endl;

		builder.SetBounds(i, i + step, j, j + step);
		builder.Build();

		renderer.Render();

		writer.SetDestFilename(name);
		writer.WriteDestFile();
	}
}

int main (int argc, char** argv)
{
	module::Perlin source;
	source.SetOctaveCount(module::PERLIN_MAX_OCTAVE);

	utils::NoiseMap map;
	utils::NoiseMapBuilderPlane builder;
	builder.SetDestSize(257, 257);
	builder.SetSourceModule (source);
	builder.SetDestNoiseMap (map);

	utils::RendererImage renderer;
	utils::Image image;
	renderer.SetSourceNoiseMap (map);
	renderer.SetDestImage (image);

	utils::WriterBMP writer;
	writer.SetSourceImage (image);
	
	for(int i = 0; i < (1 << MAX_LEVEL); i++)
	{
		for(int j = 0; j < (1 << MAX_LEVEL); j++)
		{
			for(int level = 0; level <= MAX_LEVEL; level++)
			{
				Build(builder, renderer, writer, level, i, j);
			}
		}
	}

	return 0;
}
