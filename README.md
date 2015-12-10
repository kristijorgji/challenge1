# Challenge 1
This is my solution in C# for the following challenge problem:

You are required to implement an algorithm that recursively builds JSON based on the input.
The input is given in the following form:
<node11>/<node12>/.../<node1k>
<node21>/<node22>/.../<node2n>
...
<nodem1>/<nodem2>/.../<nodeml>
where m,l,k >= 0

Sample input:
<pre>
a/b/c1
x/b
a/b/c/d
m

Output:
{
  a: {
    b: {
	  c: {
	    d: ""
	  },
	  c1: ""
	}
  },
  x: {
    b: ""
  },
  m: ""
}
</pre>

*Note that ordering of nodes in the output does not matter
**Also, first you can do preprocessing, then apply the recursive algorithm.
