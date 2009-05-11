//#version 130

uniform sampler2D noise0;
uniform sampler2D noise1;
uniform sampler2D noise2;
uniform sampler2D noise3;
uniform sampler2D noise4;

uniform float texScale;
uniform float texOffset;
uniform float heightScale;

uniform float level;

uniform vec3 eye;

in vec4 glVertex;

varying vec3 normal;
varying float bias;

vec4 calcTexCoord(vec4 vertex)
{
	return (vertex) * texScale + texOffset;
}

float getBias(vec4 texCoord)
{
	if(level == 4.0)
	{
		return texture2D(noise4, texCoord.st).r;
	}
	else if(level == 3.0)
	{
		return texture2D(noise3, texCoord.st).r;
	}
	else if(level == 2.0)
	{
		return texture2D(noise2, texCoord.st).r;
	}
	else if(level == 1.0)
	{
		return texture2D(noise1, texCoord.st).r;
	}
	else if(level == 0.0)
	{
		return texture2D(noise0, texCoord.st).r;
	}
	else
	{
		return 0.0;
	}
}

float getHeight(float bias)
{
	return heightScale * bias;
}

float getHeight(vec4 texCoord)
{
	return getHeight(getBias(texCoord));
}

void main()
{	
	float vertexScale = pow(2.0, level);
	
	vec4 vertex = (glVertex  * vertexScale);

	vec4 texCoord = calcTexCoord(vertex);
	
	bias = getBias(texCoord);
	vertex.z = getHeight(bias);
	vertex.w = 1.0;
	
	vec4 komsu1 = vertex;
	//TODO: check bounds
	komsu1.x = komsu1.x + vertexScale;
	komsu1.z = getHeight(calcTexCoord(komsu1));
	vec3 delta1 = normalize((komsu1 - vertex).xyz);
	
	vec4 komsu2 = vertex;
	//TODO: check bounds
	komsu2.y = komsu2.y + vertexScale;
	komsu2.z = getHeight(calcTexCoord(komsu2));
	vec3 delta2 = normalize((komsu2 - vertex).xyz);
	
	vec4 komsu3 = vertex;
	//TODO: check bounds
	komsu3.x = komsu3.x - vertexScale;
	komsu3.z = getHeight(calcTexCoord(komsu3));
	vec3 delta3 = normalize((komsu3 - vertex).xyz);
	
	vec4 komsu4 = vertex;
	//TODO: check bounds
	komsu4.y = komsu4.y - vertexScale;
	komsu4.z = getHeight(calcTexCoord(komsu4));
	vec3 delta4 = normalize((komsu4 - vertex).xyz);

	vec3 normal1 = cross(delta1, delta2);
	vec3 normal2 = cross(delta2, delta3);
	vec3 normal3 = cross(delta3, delta4);
	vec3 normal4 = cross(delta4, delta1);
	normal = (normal1 + normal2 + normal3 + normal4) / 4.0;

	vertex.xy += eye.xy;
	
	gl_Position = gl_ModelViewProjectionMatrix * vertex;
}
