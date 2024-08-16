using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static List<string> _values = new List<string>
        {
            "value1", "value2", "value3"
        };

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id >= 0 && id < _values.Count)
            {
                return Ok(_values[id]);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            _values.Add(value);
            return CreatedAtAction(nameof(Get), new { id = _values.Count - 1 }, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            if (id >= 0 && id < _values.Count)
            {
                _values[id] = value;
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id >= 0 && id < _values.Count)
            {
                _values.RemoveAt(id);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
